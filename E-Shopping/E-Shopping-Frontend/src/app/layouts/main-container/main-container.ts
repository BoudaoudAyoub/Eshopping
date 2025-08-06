import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Navbar } from '../../shared/navbar/navbar';

@Component({
  selector: 'app-main-container',
  imports: [RouterOutlet, CommonModule, Navbar],
  templateUrl: './main-container.html'
})
export class MainContainer {

}