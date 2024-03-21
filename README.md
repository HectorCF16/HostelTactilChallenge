# Connect Four API

This API allows you to check the outcome of a Connect Four game by passing the current board state through the URL.

## Usage

### Request

Make a GET request to `/ht/api/connect-four/{boardState}` where `{boardState}` represents the current state of the Connect Four board. Use `A` for Team A's chips, `B` for Team B's chips, and `X` for empty slots.

Example:
  GET /ht/api/connect-four/AXXXXXAXXXXXAXXXXXAXXXXXBBBXXXXXXXXXXXXXXX

### Response

The API returns a JSON response with the game outcome:
- `"A"`: Team A wins.
- `"B"`: Team B wins.
- `"X"`: Game continues or has an impossible board.

## Running Locally

1. Clone the repository:
'https://github.com/HectorCF16/HostelTactilChallenge'

2. Navigate to the project directory:
'cd HostelTactilChallenge'

3. Run the API using the .NET CLI:
'dotnet run --project HostelTactilChallenge'

4. Open in your browser:
'http://localhost:{PORT}/ht/api/connect-four/AXXXXXAXXXXXAXXXXXAXXXXXBBBXXXXXXXXXXXXXXX'

## Additional Notes

This API is for demonstration purposes and lacks extensive error handling.
