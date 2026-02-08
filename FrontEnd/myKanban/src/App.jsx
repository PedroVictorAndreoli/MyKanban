import { useEffect, useMemo, useState } from 'react'
import KanbanBoard from './components/KanbanBoard.jsx'
import KanbanForm from './components/KanbanForm.jsx'
import KanbanHeader from './components/KanbanHeader.jsx'
import LabelsService from './services/labelsService.js'
import StatusesService from './services/statusesService.js'
import TasksService from './services/tasksService.js'
import './App.css'
const apiBaseUrl = import.meta.env.VITE_API_BASE_URL

const labelsService = new LabelsService(apiBaseUrl, 'api/labels')
const statusesService = new StatusesService(apiBaseUrl, 'api/statuses')
const tasksService = new TasksService(apiBaseUrl, 'api/tasks')

function App() {
  const [statuses, setStatuses] = useState([])
  const [labels, setLabels] = useState([])
  const [tasksById, setTasksById] = useState({})
  const [isLoading, setIsLoading] = useState(true)
  const [error, setError] = useState('')
  const [formState, setFormState] = useState({
    title: '',
    description: '',
    labelId: '',
    statusId: '',
  })

  const statusOrder = useMemo(
    () => statuses.map((status) => String(status.id)),
    [statuses],
  )

  const loadData = async () => {
    setIsLoading(true)
    setError('')

    try {
      const [nextLabels, nextStatuses, nextTasks] = await Promise.all([
        labelsService.getAll(),
        statusesService.getAll(),
        tasksService.getAll(),
      ])

      setLabels(nextLabels)
      setStatuses(nextStatuses)
      setTasksById(
        nextTasks.reduce((acc, task) => {
          acc[String(task.id)] = task
          return acc
        }, {}),
      )

      setFormState((prev) => ({
        ...prev,
        labelId: nextLabels[0] ? String(nextLabels[0].id) : '',
        statusId: nextStatuses[0] ? String(nextStatuses[0].id) : '',
      }))
    } catch (loadError) {
      setError(loadError.message || 'Falha ao carregar dados.')
    } finally {
      setIsLoading(false)
    }
  }

  useEffect(() => {
    loadData()
  }, [])

  const statusOptions = useMemo(
    () =>
      statusOrder.map((statusId) => ({
        value: statusId,
        label: statuses.find((status) => String(status.id) === statusId)?.title ?? '',
      })),
    [statuses, statusOrder],
  )

  const labelOptions = useMemo(
    () => labels.map((label) => ({ value: String(label.id), label: label.name })),
    [labels],
  )

  const tasksByStatus = useMemo(() => {
    const grouped = Object.fromEntries(
      statuses.map((status) => [String(status.id), []]),
    )

    Object.values(tasksById).forEach((task) => {
      const statusKey = String(task.statusId)
      if (!grouped[statusKey]) {
        grouped[statusKey] = []
      }
      grouped[statusKey].push(task)
    })

    return grouped
  }, [tasksById, statuses])

  const rebuildTasksById = (groupedTasks) => {
    const orderedTasks = statusOrder.flatMap((statusId) =>
      groupedTasks[statusId] ?? [],
    )
    const nextTasksById = {}
    orderedTasks.forEach((task) => {
      nextTasksById[String(task.id)] = task
    })
    return nextTasksById
  }

  const handleDragEnd = async (result) => {
    const { destination, source } = result

    if (!destination) {
      return
    }

    if (
      destination.droppableId === source.droppableId &&
      destination.index === source.index
    ) {
      return
    }

    const sourceTasks = Array.from(tasksByStatus[source.droppableId])
    const [movedTask] = sourceTasks.splice(source.index, 1)

    if (destination.droppableId === source.droppableId) {
      sourceTasks.splice(destination.index, 0, movedTask)
      setTasksById(() =>
        rebuildTasksById({
          ...tasksByStatus,
          [source.droppableId]: sourceTasks,
        }),
      )
      return
    }

    const destinationTasks = Array.from(tasksByStatus[destination.droppableId])
    destinationTasks.splice(destination.index, 0, {
      ...movedTask,
      statusId: Number(destination.droppableId),
    })

    setTasksById(() =>
      rebuildTasksById({
        ...tasksByStatus,
        [source.droppableId]: sourceTasks,
        [destination.droppableId]: destinationTasks,
      }),
    )

    try {
      await tasksService.update(String(movedTask.id), {
        ...movedTask,
        statusId: Number(destination.droppableId),
      })
    } catch (updateError) {
      setError(updateError.message || 'Falha ao atualizar a tarefa.')
      await loadData()
    }
  }

  const handleChange = (event) => {
    const { name, value } = event.target
    setFormState((prev) => ({ ...prev, [name]: value }))
  }

  const handleSubmit = (event) => {
    event.preventDefault()

    if (!formState.title.trim()) {
      return
    }

    const newTask = {
      title: formState.title.trim(),
      description: formState.description.trim(),
      labelId: Number(formState.labelId),
      statusId: Number(formState.statusId),
    }
    tasksService
      .create(newTask)
      .then(() => {
        window.location.reload()
      })
      .catch((submitError) =>
        setError(submitError.message || 'Falha ao salvar tarefa.'),
      )
  }

  return (
    <main className="kanban-app">
      {error ? <p className="kanban-error">{error}</p> : null}
      <KanbanHeader
        title="Meu Kanban"
        subtitle="Crie tarefas e arraste entre colunas."
        actions={
          <button className="kanban-button secondary" type="button">
            Filtrar
          </button>
        }
      />

      {isLoading ? (
        <p>Carregando...</p>
      ) : (
        <KanbanForm
          formState={formState}
          statusOptions={statusOptions}
          labelOptions={labelOptions}
          onChange={handleChange}
          onSubmit={handleSubmit}
        />
      )}

      {isLoading ? null : (
        <KanbanBoard
          tasksByStatus={tasksByStatus}
          statusOrder={statusOrder}
          statuses={statuses}
          labels={labels}
          tasksById={tasksById}
          onDragEnd={handleDragEnd}
        />
      )}
    </main>
  )
}

export default App
