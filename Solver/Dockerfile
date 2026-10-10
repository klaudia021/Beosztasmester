FROM python:3.14-slim

WORKDIR /app

COPY pyproject.toml .
COPY app ./app

RUN python -m pip install --no-cache-dir .

CMD ["python", "-m", "uvicorn", "app.main:app", "--host", "0.0.0.0", "--port", "8000"]
