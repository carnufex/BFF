import { Route } from '@angular/router';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { UserSessionComponent } from './user-session/user-session.component';

export const appRoutes: Route[] = [
  { path: '', component: FetchDataComponent, pathMatch: 'full' },
  { path: 'user-session', component: UserSessionComponent },
];
