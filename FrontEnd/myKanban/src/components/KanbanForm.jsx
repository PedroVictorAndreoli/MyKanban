function KanbanForm({ formState, statusOptions, labelOptions, onChange, onSubmit }) {
  return (
    <form className="kanban-form" onSubmit={onSubmit}>
      <input
        className="kanban-input"
        type="text"
        name="title"
        placeholder="Título da tarefa"
        value={formState.title}
        onChange={onChange}
        required
      />
      <select
        className="kanban-select"
        name="labelId"
        value={formState.labelId}
        onChange={onChange}
      >
        {labelOptions.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
      <select
        className="kanban-select"
        name="statusId"
        value={formState.statusId}
        onChange={onChange}
      >
        {statusOptions.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
      <textarea
        className="kanban-textarea"
        name="description"
        placeholder="Descrição"
        value={formState.description}
        onChange={onChange}
        rows={2}
      />
      <button className="kanban-button" type="submit">
        Criar tarefa
      </button>
    </form>
  )
}

export default KanbanForm
