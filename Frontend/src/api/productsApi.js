import axios from 'axios'

const API = axios.create({
  baseURL: '/api/products',
})

export const getProducts = async (params = {}) => {
  const response = await API.get('/', { params })
  return response.data
}

export const getProduct = async (id) => {
  const response = await API.get(`/${id}`)
  return response.data
}

export const createProduct = async (product) => {
  const response = await API.post('/', product)
  return response.data
}

export const updateProduct = async (id, product) => {
  const response = await API.put(`/${id}`, product)
  return response.data
}

export const deleteProduct = async (id) => {
  await API.delete(`/${id}`)
}