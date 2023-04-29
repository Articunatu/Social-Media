import Profile from '../components/Profile'
import React, { useState, useEffect } from 'react';
import { Messages } from '../../controllers/service/MessageReciever';
import Post from '../components/Post';


export default function ProfileInfo() {
    const [profileInfo, setProfileInfo] = useState([]);
    const { userId } = 1;

    useEffect(() => {
    const fetchProfileInfo = async () => {
        const data = Messages.receivedData;
        setProfileInfo(data);
    };

    fetchProfileInfo();
    }, [userId]);

    const profileModel = {
        
    }

    return (
        <div className='profile-info-container'>
            <Post e={userId}></Post>
            <Post
                text={"Nu ska vi intressera oss för aktier"}
                />
            <Post
                text={"Det kan inte vara förbjudet att ogilla författare från Norge, Det kan inte vara förbjudet att ogilla författare från Norge, Det kan inte vara förbjudet att ogilla författare från Norge"}
                />
            <Post
                text={"Det kan inte vara förbjudet att ogilla författare från Norge"}
                />
            <Post
                text={"Det kan inte vara förbjudet att ogilla författare från Norge"}
                />
            <Post
                text={"Det kan inte vara förbjudet att ogilla författare från Norge"}
                />
            <Post
                text={"Det kan inte vara förbjudet att ogilla författare från Norge"}
                />
            <Post
                text={"Det kan inte vara förbjudet att ogilla författare från Norge"}
                />
        </div>
    );
}