import { DragDropContext } from '@hello-pangea/dnd'
import KanbanColumn from './KanbanColumn.jsx'

function KanbanBoard({
  tasksByStatus,
  statusOrder,
  statuses,
  tasksById,
  labels,
  onDragEnd,
}) {
  const statusMap = new Map(statuses.map((status) => [status.id, status]))
  const labelsById = Object.fromEntries(labels.map((label) => [label.id, label]))

  return (
    <DragDropContext onDragEnd={onDragEnd}>
      <section className="kanban-board" aria-label="Quadro Kanban">
        {statusOrder.map((statusId) => (
          <KanbanColumn
            key={statusId}
            status={statusMap.get(Number(statusId))}
            tasks={tasksByStatus[statusId] ?? []}
            tasksById={tasksById}
            labelsById={labelsById}
          />
        ))}
      </section>
    </DragDropContext>
  )
}

export default KanbanBoard
