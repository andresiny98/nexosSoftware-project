import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HomeService } from 'src/app/services/home.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  items: any;
  termino: string;

  estado: boolean= false;
  buscando: boolean= false;


  constructor(
    private home: HomeService
  ) {}

  ngOnInit(): void {
    this.getLibros();
  }

  getLibros() {
    this.home.getAllLibro().subscribe((resp) => {
      this.items = resp;
      if(this.items == ''){
        this.estado = true;
      }else{
        this.estado = false;
      }
     // console.log(this.items);
    });

  }

  recibeSearch(propagar:any){
   //   console.log("Se recibio la propagación");
      this.items=propagar;
      if(this.items == ''){
        this.buscando = true;
      }else{
        this.buscando = false;
      }
  }




}
