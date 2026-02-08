function KanbanHeader({ title, subtitle, actions }) {
  return (
    <header className="kanban-header">
      <div>
        <h1 className="kanban-title">{title}</h1>
        {subtitle ? <p className="kanban-subtitle">{subtitle}</p> : null}
      </div>
      {actions ? <div className="kanban-actions">{actions}</div> : null}
    </header>
  )
}

export default KanbanHeader
