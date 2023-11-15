import { Images } from "./images"

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
