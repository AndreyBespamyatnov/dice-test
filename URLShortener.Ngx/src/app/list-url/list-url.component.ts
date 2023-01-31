import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTableDataSource } from '@angular/material/table';
import { PageEvent } from '@angular/material/paginator';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';
import { AddUrlComponent } from '../add-url/add-url.component';
import { Url } from '../api.clients';
import { UrlShortenerService } from '../services/url-shortener.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-list-url',
  templateUrl: './list-url.component.html',
  styleUrls: ['./list-url.component.scss'],
})
export class ListUrlComponent implements OnInit {
  displayedColumns: string[] = ['originalUrl', 'shortUrl', 'createdAt'];
  dataSource = new MatTableDataSource<Url>();
  totalCount: number = 0;
  pageSize = 10;
  pageSizeOptions: number[] = [5, 10, 25, 100];

  filterControl = new FormControl();

  constructor(
    private dialog: MatDialog,
    private snackBar: MatSnackBar,
    private urlShortenerService: UrlShortenerService
  ) {}

  ngOnInit() {
    this.filterControl.valueChanges
      .pipe(debounceTime(300), distinctUntilChanged())
      .subscribe((filterValue) => {
        this.dataSource.filter = filterValue.trim().toLowerCase();
      });

    this.loadPage({
      pageIndex: 0, 
      pageSize: this.pageSize,
      length: 0,
    });
  }

  loadPage(event: PageEvent) {
    this.urlShortenerService
      .listUrls(event.pageIndex, event.pageSize)
      .subscribe((response) => {
        this.dataSource = new MatTableDataSource<Url>(response.data);
        this.totalCount = response.totalCount;
      });
  }

  openDialog(): void {
    const dialogRef = this.dialog.open(AddUrlComponent, {
      width: '80%',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (!result) {
        return;
      }
      
      const originalUrl = result;
      this.urlShortenerService
        .createShortUrl(originalUrl!)
        .subscribe((response) => {
          if(response.url) {
            if (this.dataSource.data.some(x => x.id == response.url?.id)){
              return;
            }

            this.dataSource.data = [response.url, ...this.dataSource.data]
            this.totalCount = this.totalCount + 1;
            this.snackBar.open(
              `Short URL created with the link: '${response.url?.shortUrl}'!`,
              '',
              { duration: 2000 }
            );
          }
        });
      }
    );
  }

  getFullShortUrl(url: string): string {
    return `${environment.api}/${url}`;
  }
}
