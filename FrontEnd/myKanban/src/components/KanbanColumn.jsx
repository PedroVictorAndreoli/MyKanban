import { Draggable, Droppable } from '@hello-pangea/dnd'
import KanbanCard from './KanbanCard.jsx'

function KanbanColumn({ status, tasks, tasksById, labelsById }) {
  return (
    <article className="kanban-column">
      <h2>{status.title}</h2>
      <Droppable droppableId={String(status.id)}>
        {(provided, snapshot) => (
          <div
            className={`kanban-column-content${snapshot.isDraggingOver ? ' dragging-over' : ''}`}
            ref={provided.innerRef}
            {...provided.droppableProps}
          >
            {tasks.map((task, index) => (
              <Draggable
                key={task.id}
                draggableId={String(task.id)}
                index={index}
              >
                {(dragProvided, dragSnapshot) => (
                  <KanbanCard
                    task={tasksById[String(task.id)]}
                    label={labelsById[task.labelId]}
                    dragProvided={dragProvided}
                    dragSnapshot={dragSnapshot}
                  />
                )}
              </Draggable>
            ))}
            {provided.placeholder}
          </div>
        )}
      </Droppable>
    </article>
  )
}

export default KanbanColumn
