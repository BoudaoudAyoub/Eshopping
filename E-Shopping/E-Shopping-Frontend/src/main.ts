import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/app.config';
import { MainContainer } from './app/layouts/main-container/main-container';

bootstrapApplication(MainContainer, appConfig)
  .catch((err) => console.error(err));
