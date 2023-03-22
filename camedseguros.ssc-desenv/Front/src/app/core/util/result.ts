import { Annotation } from './annotation';

export interface Result {
  successfully: boolean;
  message: string;
  notifications: Array<Annotation>;
  hasNotification: boolean;
  validators: any;
  payload: any;
  length: number;
}
