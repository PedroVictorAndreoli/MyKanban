import AjaxClient from './ajaxClient.js'

class TasksService extends AjaxClient {
  mapFromApi(item) {
    return {
      id: item.id,
      title: item.title,
      description: item.description,
      labelId: item.labelId,
      statusId: item.statusId,
    }
  }

  mapToApi(item) {
    return {
      id: item.id,
      title: item.title,
      description: item.description,
      labelId: item.labelId,
      statusId: item.statusId,
    }
  }
}

export default TasksService
