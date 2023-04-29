import React from 'react';
import s from '../../style/profile.module.scss';

export default class AccountView extends React.Component {
    render() {
        const { fullname, tag, profilePictureUrl, text } = this.props;
        
        return (
            <div className="container">
                <div className={s.profile_container}>
                        <img className={s.profile_picture} src={profilePictureUrl} alt="<PROFILEPICTURE>" />                 
                        <div className={s.name_container}>
                            <h2 className={s.profile_name}>Andreas Saki</h2>
                            <h3 className={s.profile_tag}>@hitomiueda</h3>
                        </div>
                    </div>  
            </div>
        )
    }
}