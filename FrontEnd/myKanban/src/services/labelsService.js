import AjaxClient from './ajaxClient.js'

class LabelsService extends AjaxClient {
  mapFromApi(item) {
    return {
      id: item.id,
      name: item.name,
      color: {
        bg: item.colorBg,
        text: item.colorText,
      },
    }
  }

 
}

export default LabelsService
