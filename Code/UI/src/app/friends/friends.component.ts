import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Friend } from '../models/friend.model';
import { FollowService } from '../services/follow.service';

@Component({
  selector: 'app-friends',
  templateUrl: './friends.component.html',
  styleUrls: ['./friends.component.css']
})
export class FriendsComponent implements OnInit {

  friends: Friend[];

  pendingRequests: Friend[];

  constructor(private _followService: FollowService,
    private _modalService: NgbModal) { }

  ngOnInit(): void {

    this.getFriends();

    this._followService.getFriendRequests().subscribe(data => {

      this.pendingRequests = data;

    });
  }

  private getFriends() {
    this._followService.getFriends().subscribe(data => {
      this.friends = data;
    });
  }

  acceptFriendRequest(friend: Friend) {
    this._followService.acceptFriendRequest(friend).subscribe(data => {

      console.log(data);

      this._followService.getFriends().subscribe(data => {
        this.friends = data;
      });

      this._followService.getFriendRequests().subscribe(data => {

        this.pendingRequests = data;
        console.log(this.pendingRequests);
      });
    });
  }

  rejectFriendRequest(friend: Friend) {
    this._followService.deleteFriendRequest(friend).subscribe(data => {
      this._followService.getFriendRequests().subscribe(data => {
        this.pendingRequests = data;
      });
    });
  }

  unfriend(friend: Friend) {
    this._followService.unFriend(friend).subscribe(data => {
      this.getFriends();
    });
  }

  UnfriendConfirmation(friend: Friend) {
    const modal = this._modalService.open(NgbdModalConfirm);
    modal.componentInstance.friendBase = friend;
    modal.componentInstance.clickevent.subscribe(($e) => {
      this.unfriend($e);
    })
  }
}


@Component({
  selector: 'ngbd-modal-confirm',
  template: `
  <div class="modal-header">
    <h4 class="modal-title" id="modal-title">Unfriend</h4>
    <button type="button" class="close" aria-describedby="modal-title" (click)="modal.dismiss('Cross click')">
      <span aria-hidden="true">&times;</span>
    </button>
  </div>
  <div class="modal-body">
    <p><strong>Are you sure you want to unfriend <span class="text-primary">{{friendBase.UserName}}</span>?</strong></p>
    <p><span class="text-danger">This operation can not be undone.</span></p>
  </div>
  <div class="modal-footer">
    <button type="button" class="btn btn-outline-secondary" (click)="modal.dismiss('cancel click')">Cancel</button>&nbsp;
    <button type="button" class="btn btn-primary" (click)="Ok(friendBase)">Yes</button>
  </div>
  `
})
export class NgbdModalConfirm {

  @Output() clickevent = new EventEmitter<Friend>();

  constructor(public modal: NgbActiveModal) { }
  friendBase: Friend;

  Ok(friend: Friend) {
    this.clickevent.emit(friend);
    this.modal.dismiss();
  }
}

