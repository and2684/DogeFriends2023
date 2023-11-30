import { Images } from "./Images";

export interface DogDto {
  id: number;
  name: string;
  dogBreedId: number;
  dogBreed: string;
  dogUser: string;
  dogUsername: string;
  base64Image: string;
  externalId: string;
}

export interface DogDtoDetailed extends DogDto {
  images: Images
}
