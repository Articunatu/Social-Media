export interface LoginCommand {
    tag: string;
    password: string;
}

export interface SignUpCommand {
    tag: string;
    email: string;
    firstName?: string;
    lastName?: string;
    password: string;
}

export interface LogoutCommand {
    userId: string;
}

export interface LoginResponse {
    accessToken: string;
    refreshToken: string;
}

export interface RefreshTokenCommand {
    refreshToken?: string;
}

export interface ChangePasswordCommand {
    userId: string;
    oldPassword: string;
    newPassword: string;
    confirmPassword: string;
}

export interface AuthorizeResponse {
    userId: string;
    username: string;
}
