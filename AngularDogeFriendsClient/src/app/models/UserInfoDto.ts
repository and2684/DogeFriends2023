import { IImage } from "./Images";

export interface UserInfoDto {
  id: number;
  username: string;
  firstName: string;
  lastName: string;
  showname: string;
  email: string;
  hometown: string | null;
  description: string | null;
  externalId: string;
}

export interface UserInfoDtoWithMainImage extends UserInfoDto {
  mainImage?: string | null;
}

