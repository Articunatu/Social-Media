function getBaseApiUrl(version: number): string {
    // Provide the actual implementation
    // For example: return `https://api.example.com/v${version}/`;
    return '';
}


export const API = {        
        feed: {
        react() {
            return `${getBaseApiUrl(1)}reaction/save`;
        },
        vote() {
            return `${getBaseApiUrl(3)}feed/vote`;
        }
    },
}