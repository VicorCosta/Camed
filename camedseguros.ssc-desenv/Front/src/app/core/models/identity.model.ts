import { User } from './user.model';

export interface Identity {
  authenticated: boolean;
  accessToken: string;
  expiration: Date;
  user: User;
}
