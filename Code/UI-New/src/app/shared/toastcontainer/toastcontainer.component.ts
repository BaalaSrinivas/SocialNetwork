import { Component, OnInit, TemplateRef } from '@angular/core';
import { ToastService } from '../../services/toast.service';

@Component({
  selector: 'app-toasts',
  template: `<div>
    <ngb-toast
             *ngFor="let toast of toastService.toasts"
             class = "bg-light {{toast.class}}"
    [autohide] = "true"
    [delay] = "toast.delay || 5000"
      (hidden) = "toastService.remove(toast)"
      [header] = "toast.title"
      >
  {{toast.content}}
  </ngb-toast>
  </div>`
  ,
  styleUrls: ['./toastcontainer.component.css'],
  host: { '[class.ngb-toasts]': 'true' }
})
export class ToastcontainerComponent implements OnInit {

  constructor(public toastService: ToastService) { }

  ngOnInit(): void {
  }
  isTemplate(toast) {
    return toast.textOrTpl instanceof TemplateRef;
  }
}
