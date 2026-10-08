import React, { useState } from "react";
import { createRoot } from "react-dom/client";
import "./styles.css";

type AskResult = {
  answer: string;
  sources: string[];
};

function App() {
  const [question, setQuestion] = useState("");
  const [result, setResult] = useState<AskResult | null>(null);
  const [loading, setLoading] = useState(false);

  async function ask() {
    setLoading(true);
    setResult(null);

    try {
      const response = await fetch("http://localhost:5000/api/ask", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ question }),
      });

      if (!response.ok) throw new Error("Request failed");
      setResult(await response.json());
    } finally {
      setLoading(false);
    }
  }

  return (
    <main className="page">
      <section className="card">
        <p className="eyebrow">.NET + React + LLM</p>
        <h1>AI Knowledge Assistant</h1>
        <p className="subtitle">
          Ask a question and receive an answer grounded in a small enterprise knowledge base.
        </p>

        <label htmlFor="question">Question</label>
        <textarea
          id="question"
          rows={5}
          value={question}
          onChange={(event) => setQuestion(event.target.value)}
          placeholder="What is required before a production release?"
        />

        <button disabled={loading || question.trim().length < 3} onClick={ask}>
          {loading ? "Thinking..." : "Ask assistant"}
        </button>

        {result && (
          <section className="result">
            <h2>Answer</h2>
            <p>{result.answer}</p>

            <h3>Sources</h3>
            <ul>
              {result.sources.map((source) => (
                <li key={source}>{source}</li>
              ))}
            </ul>
          </section>
        )}
      </section>
    </main>
  );
}

createRoot(document.getElementById("root")!).render(<App />);
