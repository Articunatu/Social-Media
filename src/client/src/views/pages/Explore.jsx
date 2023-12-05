export default function Explore() {
    const search = false;
    if(search) {
        return(
            <div className="search-container"></div>
        )
    }
    else {
        return(
            <div className="explore-container"></div>
        )
    }
}