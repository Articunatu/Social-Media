export interface LoginCommand {
    tag: string;
    password: string;
}

export interface SignUpCommand {
    username: string;
    password: string;
    email: string;
}

export interface LogoutCommand {
    userId: string;
}

export interface LoginResponse {
    accessToken: string;
    refreshToken: string;
}

export interface RefreshTokenCommand {
  // If needed, add properties
}

export interface ChangePasswordCommand {
    userId: string;
    oldPassword: string;
    newPassword: string;
}

export interface AuthorizeResponse {
    userId: string;
    username: string;
}