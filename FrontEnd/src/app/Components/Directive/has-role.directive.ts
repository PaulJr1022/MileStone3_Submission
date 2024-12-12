import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[appHasRole]'
})
export class HasRoleDirective {

  private currentRole : string ='';

  constructor(private templateRef : TemplateRef<any>, private viewContainer: ViewContainerRef ) { }

  @Input() set appHasRole (role : string){
    this.currentRole = role;
    this.updateView();
  }

  private updateView(){
    let Role = localStorage.getItem('Role');
    if(Role == this.currentRole){
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else{
      this.viewContainer.clear();
    }
  }

}
