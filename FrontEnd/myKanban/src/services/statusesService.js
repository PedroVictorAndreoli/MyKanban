import AjaxClient from './ajaxClient.js'

class StatusesService extends AjaxClient {
  mapFromApi(item) {
    return {
      id: item.id,
      title: item.title,
    }
  }
}

export default StatusesService
