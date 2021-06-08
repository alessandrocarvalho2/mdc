export class TokenModel {
  accessToken?: string;
  authenticated?: boolean;
  created?: Date;
  expiration?: Date;
  message?: string;
  refreshToken?: string;
}

export default TokenModel;
