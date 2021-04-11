import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { SectionsModule } from "../sections/sections.module";
import { FeedSectionComponent } from "./feed-section/feed-section.component";
import { NewsfeedComponent } from "./newsfeed.component";

@NgModule({
    imports: [
        CommonModule,
        BrowserModule,
        FormsModule,
        NgbModule,
        RouterModule,
        SectionsModule
    ],
    declarations: [NewsfeedComponent, FeedSectionComponent],
    exports: [NewsfeedComponent],
    providers: []
})
export class NewsFeedModule { }