import React from 'react';
import s from '../../style/post-message-box.scss';
import Profile from './Profile';
import Fab from '@mui/material/Fab';
import Button from './general/Button'
import AddIcon from '@mui/material/Fab';
import { Icon } from '@mui/material';
import { Card } from '@mui/material';
import { ButtonGroup } from '@mui/material';
import StarOutlineOutlinedIcon from '@mui/icons-material/StarOutlineOutlined';
import Grid from '@mui/material/Grid'; // Grid version 1
import CommentIcon from '@mui/icons-material/Comment';

export default class Post extends React.Component {

    render() {
        const { fullname, tag, profilePictureUrl, text } = this.props;

        const openComment = () => {
            
        }

        return (
            <Card variant="outlined">
                <div className={s.post_content}>
                {/* <button className='Share'>Dela</button> */}
                    <Profile></Profile>
                    <div className={s.post_text}>
                        <p>{text}</p>
                    </div>
                </div>
                
                <div className={s.button_list}>
                    <Grid container spacing={{ xs: 12, md: 0 }} columns={{ xs: 4, sm: 1, md: 12 }}>
                        <Grid xs={4} sm={8} md={4}>
                            <Button color="primary" variant="contained" label="REACT"></Button>
                        </Grid>
                        <Grid xs={4} sm={8} md={4}>
                            <Button color="secondary" variant="contained" label="SHARE"></Button>
                        </Grid>
                        <Grid xs={4} sm={8} md={4}>
                            <Button color="warning" variant="contained" label="REPLY"></Button>
                        </Grid>
                        <CommentIcon onclick={openComment}></CommentIcon>
                    </Grid>
                </div>
            </Card>
        );
    }
}