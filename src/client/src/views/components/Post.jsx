import React from 'react';
import s from './../../style/post-message-box.scss';
import Profile from './Profile';

export default class Post extends React.Component {
    render() {
        const { fullname, tag, profilePictureUrl, text } = this.props;

        return (
            <div className={s.post_container}>

                <div className={s.post_content}>
                <button className='Share'>Dela</button>
                    <Profile></Profile>
                    <div className={s.post_text}>
                        <p>{text}</p>
                    </div>
                </div>
                <div className={s.reaction_conatiner}>
                    {/* <ReactionList></ReactionList> */}
                    <div className={s.buttons}>
                        <button className='Comment'>Kommentera</button>
                        <button className='React'>Reagera</button>
                    </div>
                </div>
            </div>
        );
    }
}