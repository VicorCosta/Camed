import { Component, Output, EventEmitter, Input, OnChanges, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { BaseComponent, AuthenticationService } from 'src/app/core';
import { FormBuilder } from '@angular/forms';
import { AuditoriaService } from '../service';
import { AuditoriaDetalhesService } from '../service';
import * as moment from 'moment';

@Component({
  selector: 'app-print-auditoria',
  templateUrl: './print-auditoria.component.html',
  styleUrls: ['./print-auditoria.component.css']
})
export class PrintAuditoriaComponent extends BaseComponent implements OnChanges, OnInit {

  submitted = false;
  display = false;
  titulo: string;
  odataUrl: any;
  mostrar = false;

  detalhes: any = [];

  @Output() eventoConcluido = new EventEmitter<any>();
  @Output() closePrint = new EventEmitter<any>();
  @Input() item: any;
  @Input() impressao: any;


  constructor(authenticationService: AuthenticationService,
    fb: FormBuilder,
    route: ActivatedRoute,
    router: Router,
    private service: AuditoriaService,
    private auditlog: AuditoriaDetalhesService) {
    super(authenticationService, fb, route, router);
    this.route.params.subscribe(params => this.odataUrl = params.odataUrl );

  }

  ngOnInit(): void {
    this.service.get(this.odataUrl).subscribe((data) => {
      this.impressao = data
      this.auditlog.get("RecordId=550992&DataInicial=2022-04-04&DataFinal=2022-10-10").subscribe((data) => {
        this.detalhes = data;
      });
    });
  }

  transform(date, format) {
    return moment(date).format(format);
  }

  ngOnChanges() {
  }

  onClosePanel() {
    this.closePrint.emit(true);
  }

  imprimir() {
    var printContents = document.getElementById('imprimir').innerHTML;

    if (window) {
      if (navigator.userAgent.toLowerCase().indexOf('chrome') > -1) {
        var popup = window.open('', '_blank',
          'width=600,height=600,scrollbars=no,menubar=no,toolbar=no,'
          + 'location=no,status=no,titlebar=no');

        popup.window.focus();
        popup.document.write('<!DOCTYPE html><html><head>'
          + '</head><body onload="window.print()"><div class="reward-body">'
          + printContents + '</div></html>');
        popup.onbeforeunload = function (event) {
          popup.close();
          return '.\n';
        };
        popup.onabort = function (event) {
          popup.document.close();
          popup.close();
        }
      } else {
        var popup = window.open('', '_blank', 'width=800,height=600');
        popup.document.open();
        popup.document.write('<html><head>' +
          +'</head><body onload="window.print()">' + printContents + '</html>');
        popup.document.close();
      }

      popup.document.close();
    }
    return true;

  }

}



