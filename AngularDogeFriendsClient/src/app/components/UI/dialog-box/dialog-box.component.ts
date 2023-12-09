import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-box',
  template: `<h2 mat-dialog-title>Удаление породы</h2>
             <mat-dialog-content>Вы уверены, что хотите удалить эту породу?</mat-dialog-content>
             <mat-dialog-content>ВНИМАНИЕ: будут удалены все привязки породы к группам и вся информация о собаках этой породы!</mat-dialog-content>
             <mat-dialog-actions align="end">
             <button mat-button mat-dialog-close color="primary">Отмена</button>
             <button mat-button (click)="confirm()" color="warn">Удалить</button>
             </mat-dialog-actions>`,
  styleUrls: ['./dialog-box.component.css']
})
export class DialogBoxComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<DialogBoxComponent>) { }

  ngOnInit() {
  }

  confirm() {
    // Пользователь нажал "Удалить"
    this.dialogRef.close(true);
  }
}
