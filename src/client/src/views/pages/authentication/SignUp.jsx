import React, { useState } from 'react';
import { URL } from '../../../Url';
import Button from '../../components/general/Button';

export default function SignUp(props) {
    const initialFormData = Object.freeze({
        title: "Account x",
        content: "This is account x of the name ok"
    });
    const [formData, setFormData] = useState(initialFormData);

    const handleChange = (e) => {
        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        const accountToCreate = {
            accountID: 0,
            title: formData.title,
            content: formData.content
        };

        const url = URL.signup;

        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(accountToCreate)
        })
            .then(response => response.json())
            .then(responseFromServer => {
                console.log(responseFromServer);
            })
            .catch((error) => {
                console.log(error);
                alert(error);
            });

        props.onAccountCreated(accountToCreate);
    };

    return (
        <div>
            <form className='w-100 px-5'>
                <h1 className='mt-5'>Create new account</h1>
                <div className='mt-5'>
                    <label className='h3 form-label'>Konto förnamn</label>
                    <input value={formData.title} name="title" type="text" className='form-control' onChange={handleChange} />
                </div>
                <div className='mt-5'>
                    <label className='h3 form-label'>Konto efternamn</label>
                    <input value={formData.content} name="content" type="text" className='form-control' onChange={handleChange} />
                </div>
                <Button onClick={handleSubmit} color="primary">Skicka</Button>
                <Button onClick={() => props.onAccountCreated(null)} color="secondary">Cancel</Button>
            </form>
        </div>
    );
}