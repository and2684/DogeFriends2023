export interface UserLoginResponseDto {
  message: string;
  isSuccess: boolean;
  errors: string[] | null;
  accessToken: string;
  refreshToken: string;
}
