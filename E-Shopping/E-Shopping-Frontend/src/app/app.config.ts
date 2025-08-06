import { provideHttpClient } from '@angular/common/http';
import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(),
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
     provideRouter([
        { 
          path: 'orders',
          loadComponent: () => import('./features/order-component/order-component').then(m => m.OrdersComponent)
        },
        { 
          path: '**',
          loadComponent: () => import('./features/home-component/home-component').then(m => m.HomeComponent)
        }
      ]),
  ]
};