import React from "react"

export default class Followers extends React.Component(id) {
    render() {

        const [apiData, setApiData] = useState([]);

        useEffect(() => {
            const fetchData = async () => {
            const data = await accountService.getFollowers(id);
            setApiData(data);
            };
            fetchData();
        }, []);

        return (
            <div>
                <p>Meddelande</p>
                <Comment/>
            </div>
        )
    }
};