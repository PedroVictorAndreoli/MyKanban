function KanbanCard({ task, label, dragProvided, dragSnapshot }) {
  const tagStyle = label?.color
    ? { background: label.color.bg, color: label.color.text }
    : undefined

  return (
    <div
      className={`kanban-card${dragSnapshot.isDragging ? ' dragging' : ''}`}
      ref={dragProvided.innerRef}
      {...dragProvided.draggableProps}
      {...dragProvided.dragHandleProps}
    >
      <span className="kanban-tag" style={tagStyle}>
        {label?.name ?? 'Sem etiqueta'}
      </span>
      <h3>{task.title}</h3>
      {task.description ? <p>{task.description}</p> : null}
    </div>
  )
}

export default KanbanCard
