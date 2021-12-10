import { IAuthenticationState } from './Authentication';
import { ISearchState } from './Search';

export interface IRootState {
  authentication: IAuthenticationState;
  journals: ISearchState[];
}