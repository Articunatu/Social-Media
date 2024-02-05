function getBaseApiUrl(version: number): string {
    // Provide the actual implementation
    // For example: return `https://api.example.com/v${version}/`;
    return '';
}


export const API = {
    quickQuestions: {
        getQuestions() {
            // return `${getBaseApiUrl(1)}quickquestion/questions`;
        },
        getQuestionById(id : string) {
            return `${getBaseApiUrl(1)}quickquestion/question/${id}`;
        },
        postQuestion() {
            return `${getBaseApiUrl(1)}quickquestion/answer`;
        },
        reportAnswer(answerId : string) {
            return `${getBaseApiUrl(1)}quickquestion/answer/report/${answerId}`;
        }
        },
        
        feed: {
        react() {
            return `${getBaseApiUrl(1)}reaction/save`;
        },
        vote() {
            return `${getBaseApiUrl(3)}feed/vote`;
        }
    },
}