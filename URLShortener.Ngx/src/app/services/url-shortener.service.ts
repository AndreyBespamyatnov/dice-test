import { Injectable } from '@angular/core';
import { Observable, tap, catchError } from 'rxjs';
import {
  Client,
  CreateShortUrlRequest,
  CreateShortUrlResponse,
  ListUrlsResponseOfUrl,
} from '../api.clients';
import { BaseService } from './base.service';

@Injectable({
  providedIn: 'root',
})
export class UrlShortenerService extends BaseService {
  constructor(private client: Client) {
    super();
  }

  createShortUrl(originalUrl: string): Observable<CreateShortUrlResponse> {
    return this.client
      .apiUrlPost({ originalUrl: originalUrl } as CreateShortUrlRequest)
      .pipe(
        tap((_) => {
          this.log(`ShortUrl has been created or updated: ${JSON.stringify(_)}`);
        }),
        catchError(
          this.handleError<CreateShortUrlResponse>(
            `Error on creating a new short URL`
          )
        )
      );
  }

  listUrls(pageNumber: number | undefined, pageSize: number | undefined): Observable<ListUrlsResponseOfUrl> {
    return this.client
      .apiUrlGet(pageNumber, pageSize)
      .pipe(
        tap((_) => {
          this.log(`Lis of URLs has been loaded: ${JSON.stringify(_)}`);
        }),
        catchError(
          this.handleError<ListUrlsResponseOfUrl>(
            `Error on getting a list of URLs`
          )
        )
      );
  }
}
