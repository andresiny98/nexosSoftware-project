import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HomeService } from 'src/app/services/home.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {

  items: any;

  @Output() propagar = new EventEmitter<any>();

  constructor( private home: HomeService,
    private activatedRoute: ActivatedRoute,
    private route: Router,) { }

  ngOnInit(): void {
  }

  searchLibro(termino: string){
    if (termino) {
      this.home.getFilterLibro(termino).subscribe((resp: any) => {
          console.log(resp)
        this.items = resp;
        this.propagar.emit(this.items);
      });
  }
}

reloadCurrentPage() {
  window.location.reload();
 }

}
