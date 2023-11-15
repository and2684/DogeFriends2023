export interface IImage {
  id: string
  uid: string
  entityName: string
  base64Data: string
  isMain: boolean
}

export type Images = IImage[]
