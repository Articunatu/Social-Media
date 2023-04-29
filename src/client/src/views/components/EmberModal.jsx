import React from "react"
// import Example from "../popup/Example";

// export default class Modal extends React.Component {
//     render() {
//         const { modalIsOpen, afterOpenModal, closeModal, customStyles, subtitle} = this.props;

//     closeText: computed('closeLinkText', 'intl', {
//     get() {
//       if (this.closeLinkText) {
//         return this.closeLinkText;
//       }

//       return this.intl.t('close');
//     },
//   }),

//   didInsertElement() {
//     this._super(...arguments);
//     const modal = this;

//     $('#mp-body').css('overflow', 'hidden');

//     scheduleOnce('afterRender', this, this.handleKeyUp, modal);
//   },

//   handleKeyUp(modal) {
//     $(document).on('keyup.modal', (e) => {
//       if (e.keyCode === 27) {
//         modal.send('close');
//       }
//     });
//   }

//   didDestroyElement() {
//     this._super(...arguments);
//     $('#mp-body').css('overflow', 'visible');
//     $(document).off('keyup.modal');
//   }

//   close() {
//     this.close();
//   },

//   clickOutside() {
//     if (this.clickOutsideClose) this.close();
//   }

//         return(
//             <EmberWormhole @to="mp-body">
//                 <div class="mp-modal-fog">
//                     <ClickOutside @onClickOutside={{action "clickOutside"}} class="mp-modal-wrapper">
//                         <div class="mp-modal-container {{this.cssClass}}">
//                             {{yield (hash
//                             header=(component "mp-modal-header" close=(action "close"))
//                             content=(component "mp-modal-content")
//                             footer=(component "mp-modal-footer" close=(action "close") closeLinkText=this.closeText stickyFooter=this.stickyFooter)
//                             )}}
//                         </div>
//                     </ClickOutside>
//                 </div>
//             </EmberWormhole>
//         )
//     }
// };