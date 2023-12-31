import { Images } from "./Images"

export interface IBreed {
  id: number
  name: string
  description: string
  coat: string
  size: string
  breedGroups: string
  externalId: string
  images: Images
}

export interface IBreedUpdate {
  name: string
  description: string
  coat: string
  size: string
  breedGroups: string,
  images?: Images
}
