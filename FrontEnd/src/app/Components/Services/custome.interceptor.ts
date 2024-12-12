import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class MyInterceptor implements HttpInterceptor {
  constructor(private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const clonedRequest = req.clone({
      setHeaders: {
        Authorization: `Bearer ${localStorage.getItem('token') || ''}`,
      },
    });

    return next.handle(clonedRequest).pipe(
      catchError((err) => {
        if ([401, 403].includes(err.status)) {
          this.router.navigate(['/home']);

        }
        const error = err.error?.message || err.statusText || `HTTP ${err.status}: ${err.statusText}`;
        console.error('HTTP Error:', err);
        return throwError(() => new Error(error));
      })
    );
  }
}
