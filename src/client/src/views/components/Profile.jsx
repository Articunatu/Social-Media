import React from 'react';
import { Link } from 'react-router-dom';
import s from '../../style/profile.module.scss';
import Container from '@mui/material/Container';
import { Avatar } from '@mui/material';
import { Badge } from '@mui/material';

export default class Profile extends React.Component {
    render() {
        const { fullname, tag, profilePictureUrl, userId } = this.props;
        const profileLink = "/profile/" + userId;

        return (
            <Container maxWidth="lg">
                <div className={s.profile_container}>
                    <Link to={profileLink}>
                    <Badge
                        overlap="circular"
                        anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
                        variant="dot"
                    ></Badge>
                        <img className={s.profile_picture} src={profilePictureUrl} alt="<PROFILEPICTURE>" />
                    </Link>
                    <div className={s.name_container}>
                        <Link to={profileLink}>
                            <h2 className={s.profile_name}>Andreas Saki</h2>                            
                        </Link>
                        <h3 className={s.profile_tag}>@hitomiueda</h3>
                    </div>
                </div>
            </Container>
        );
    }
}