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
