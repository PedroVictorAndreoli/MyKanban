class AjaxClient {
  constructor(baseUrl, resourcePath) {
    this.baseUrl = baseUrl.replace(/\/$/, '')
    this.resourcePath = resourcePath.replace(/^\//, '')
  }

  get url() {
    return `${this.baseUrl}/${this.resourcePath}`
  }

  async request(path = '', options = {}) {
    const response = await fetch(`${this.url}${path}`, {
      headers: { 'Content-Type': 'application/json' },
      ...options,
    })

    if (!response.ok) {
      const message = await response.text()
      throw new Error(message || `HTTP ${response.status}`)
    }

    if (response.status === 204) {
      return null
    }

    return response.json()
  }

  mapFromApi(item) {
    return item
  }


  async getAll() {
    const data = await this.request('')
    return data.map((item) => this.mapFromApi(item))
  }

  async create(payload) {
    const data = await this.request('', {
      method: 'POST',
      body: JSON.stringify(this.mapFromApi(payload)),
    })
    return data ? this.mapFromApi(data) : null
  }

  async update(id, payload) {
    const data = await this.request(`/${id}`, {
      method: 'PUT',
      body: JSON.stringify(this.mapFromApi(payload)),
    })
    return data ? this.mapFromApi(data) : null
  }
}

export default AjaxClient
