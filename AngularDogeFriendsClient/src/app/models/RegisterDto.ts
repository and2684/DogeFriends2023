export interface RegisterDto {
  username: string;
  password: string;
  confirmPassword: string;
  email: string;
  firstName: string;
  lastName: string;
  hometown: string | null;
  description: string | null;
}
