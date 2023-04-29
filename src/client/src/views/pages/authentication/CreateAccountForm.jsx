import React, {useState} from 'react'
import Constants from '../Constants'

export default function AccountCreateForm(props){
    const [formData, setFormData] = useState(initialFormData);

    const initialFormData = Object.freeze({
        title: "Accoutn x",
        content: "This is account x of the name ok"
    });

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

        const url = Constants.API_URL_CREATE_ACCOUNT;

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
                    <input value={formData.title} name="title" type="text" className='form-control' onChange={handleChange} />
                </div>
                <button onClick={handleSubmit} className="btn btn-dark btn-lg w-100 mt-5">Skicka</button>
                <button onClick={() => props.onAccountCreated(null)} className="btn btn-secondary btn-lg w-100 mt-3">Cancel</button>
            </form>
        </div>
    )
}